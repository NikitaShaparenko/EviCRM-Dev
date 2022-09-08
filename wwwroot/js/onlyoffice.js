function job_description_loader(string jsi_placeholder, string jsi_filetype, string jsi_key, string jsi_title, string jsi_url, string jsi_documenttype)
{
new DocsAPI.DocEditor(jsi_placeholder, {
    "document": {
        "fileType": jsi_filetype,
        "key": jsi_key,
        "title": jsi_title,
        "url": jsi_url
    },
    "documentType": jsi_documenttype
});
}