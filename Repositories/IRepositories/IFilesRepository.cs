using FileManagment.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public interface IFilesRepository
    {
        public List<Folders> GetAllFiles(int folderId);
        public FileBase GetFileById(int fileId);
        public FileBase Insert(int folderId, IFormFile obj);
        public FileBase Delete(int fileId);
        
        public FileBase Rename(int fileId, string name);

        public FileBase ReplaceFile(int folderId, IFormFile obj);
    }
}
