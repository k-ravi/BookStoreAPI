using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.WASM.Services
{
    //public class FileUpload : IFileUpload
    //{
    //    readonly IWebHostEnvironment _webHost;
    //    public FileUpload(IWebHostEnvironment webHost)
    //    {
    //        _webHost = webHost;
    //    }

    //    public void RemoveFile(string picName)
    //    {
    //        var path = $"{_webHost.WebRootPath}\\uploads\\{picName}";
    //        if (File.Exists(path))
    //        {
    //            File.Delete(path);
    //        }
    //    }

    //    public void UploadFile(IFileListEntry file, MemoryStream ms, string picName)
    //    {
    //        try
    //        {
    //            var path = $"{_webHost.WebRootPath}\\uploads\\{picName}";
    //            using (FileStream fs =new FileStream(path,FileMode.Create))
    //            {
    //                ms.WriteTo(fs);
    //            }
    //        }
    //        catch
    //        {
    //        }
    //    }
    //}
}
