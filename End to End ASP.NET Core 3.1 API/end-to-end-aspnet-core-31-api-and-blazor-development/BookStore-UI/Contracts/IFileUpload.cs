using BlazorInputFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore_UI.Contracts
{
    public interface IFileUpload
    {
        void UploadFile(IFileListEntry file,System.IO.MemoryStream ms, string picName);
        void RemoveFile(string picName);
    }
}
