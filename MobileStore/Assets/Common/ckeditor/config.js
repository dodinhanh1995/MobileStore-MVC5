/*
Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http=//ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example=
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.language = 'vi';

    config.filebrowserBrowseUrl = '/Assets/Common/ckfinder/ckfinder.html',
    config.filebrowserImageBrowseUrl = '/Assets/Common/ckfinder/ckfinder.html?type=Images',
    //config.filebrowserFlashBrowseUrl = '/Assets/Common/ckfinder/ckfinder.html?type=Flash',
    config.filebrowserUploadUrl = '/Assets/Common/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files',
    config.filebrowserImageUploadUrl = '/Assets/Common/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images'
    //config.filebrowserFlashUploadUrl = '/Assets/Common/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash'
}
