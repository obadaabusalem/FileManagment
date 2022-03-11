using FileManagment.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesRepository _FilesRepository;
        public FilesController(IFilesRepository FilesRepository)
        {
            this._FilesRepository = FilesRepository;
        }

        [HttpGet]
        public IActionResult GetAllFiles(int folderId)
        {
            var lstfiles = _FilesRepository.GetAllFiles(folderId);
            return Ok(lstfiles);
        }

        [HttpGet("{id}")]
        public FileBase GetFileById(int id)
        {
            FileBase InfoObj = new FileBase();
            InfoObj = _FilesRepository.GetFileById(id);
            return InfoObj;
        }

        [HttpPost]
        public FileBase UploadFile(int folderId,IFormFile uploadedFile)
        {
            FileBase InfoObj = new FileBase();
           // var InsertObj = new Files { FoldersID = obj.FoldersID, FileName = obj.FileName, FileData = obj.FileData };

            InfoObj = _FilesRepository.Insert(folderId, uploadedFile);
            return InfoObj;
        }

        [HttpDelete("{id}")]
        public FileBase DeleteFile(int id)
        {
            FileBase InfoObj = new FileBase();

            InfoObj = _FilesRepository.Delete(id);
            return InfoObj;
        }

        [HttpPut("{fileId},{name}")]
        public FileBase RenameFileName(int fileId,string name)
        {
            FileBase InfoObj = new FileBase();

            InfoObj = _FilesRepository.Rename(fileId, name);
            return InfoObj;
        }

        [HttpPut]
        public FileBase ReplaceFile(int folderId, IFormFile uploadedFile)
        {
            FileBase InfoObj = new FileBase();

            InfoObj = _FilesRepository.ReplaceFile(folderId, uploadedFile);
            return InfoObj;
        }
    }
}
