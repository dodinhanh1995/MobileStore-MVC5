using Models.DAO.Repository;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using static Common.Enum.EDatabase;

namespace MobileStore.Areas.Admin.Controllers
{
    public class SlideController : BaseController
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            return View(_unitOfWork._SlideRepo.Select(orderBy: x => x.OrderByDescending(s => s.Id)));
        }

        [HttpPost]
        public ActionResult Delete(FormCollection form)
        {
            string[] slideIds = form["slideId"].Split(',');

            foreach (string item in slideIds)
            {
                try
                {
                    _unitOfWork._SlideRepo.Delete(int.Parse(item));
                }
                catch
                {
                }

            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            DropDownList(selected: ESlidePosition.SlideShow);
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Slide slide)
        {
            if (ModelState.IsValid)
            {
                if (_unitOfWork._SlideRepo.CheckNameIsExists(slide.Name))
                    ModelState.AddModelError("Name", "Tên quảng cáo đã tồn tại");
                else
                {
                    try
                    {
                        _unitOfWork._SlideRepo.Insert(slide);
                        _unitOfWork.Save();
                        SetAlert("Tạo mới quảng cáo <b>" + slide.Name + "</b> thành công.", Common.Enum.EAlertMessage.Success);
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Có lỗi xảy ra khi tạo mới quảng cáo! Vui lòng thử lại.");
                    }
                }
            }
            DropDownList((ESlidePosition)Enum.Parse(typeof(ESlidePosition), slide.Position, true), slide.Target);
            return View(slide);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Slide slide = _unitOfWork._SlideRepo.SelectById(id);
            if (slide == null)
            {
                ViewBag.ErrorMessage = "Thông tin quảng cáo không tồn tại.";
                return View("Error");
            }
            DropDownList((ESlidePosition)Enum.Parse(typeof(ESlidePosition), slide.Position, true), slide.Target);
            return View(slide);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(int? id, FormCollection form)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var slide = _unitOfWork._SlideRepo.SelectById(id);
            string oldName = slide.Name;
            if (TryUpdateModel(slide, new string[] { "Name", "Image", "DisplayOrder", "Link", "Target", "Position", "Description", "Status" }))
            {
                if (_unitOfWork._SlideRepo.CheckNameIsExists(slide.Name) && oldName != slide.Name)
                    ModelState.AddModelError("Name", "Tên quảng cáo đã tồn tại");
                else
                {
                    try
                    {
                        _unitOfWork._SlideRepo.Update(slide);
                        _unitOfWork.Save();
                        SetAlert("Cập nhật thông tin quảng cáo <b>" + slide.Name + "</b> thành công.", Common.Enum.EAlertMessage.Success);
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin! Vui lòng thử lại.");
                    }
                }
            }
            DropDownList((ESlidePosition)Enum.Parse(typeof(ESlidePosition), slide.Position, true), slide.Target);
            return View(slide);
        }

        public string ChangeImage(int? id, string image)
        {
            if (id == null)
                return "Mã quảng cáo không tồn tại!";
            return _unitOfWork._SlideRepo.ChangingImage(id.Value, image);
        }

        public void TargetList(string selected = "_self")
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Mở trong trang", Value = "_self", Selected = selected == "_self" });
            items.Add(new SelectListItem { Text = "Mở trang con", Value = "_blank", Selected = selected == "_blank" });
            ViewBag.Target = items;
        }

        public void PostionDropDownList(ESlidePosition selected)
        {
            IEnumerable<ESlidePosition> values = Enum.GetValues(typeof(ESlidePosition)).Cast<ESlidePosition>();

            ViewBag.Position = from value in values
                               select new SelectListItem
                               {
                                   Text = value.ToString(),
                                   Value = value.ToString(),
                                   Selected = value == selected
                               };
        }

        public void DropDownList(ESlidePosition selected, string targetSelected = "_self")
        {
            TargetList(targetSelected);
            PostionDropDownList(selected);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}