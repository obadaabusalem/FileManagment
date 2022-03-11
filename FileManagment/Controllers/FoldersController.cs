using FileManagment.Model;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FileManagment.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly IFoldersRepository _FoldersRepository;
        public FoldersController(IFoldersRepository FoldersRepository)
        {
            this._FoldersRepository = FoldersRepository;
        }

        // GET: api/<FoldersController>
        [HttpGet]
        public IActionResult GetAllFolders()
        {
            var lstfolders= _FoldersRepository.Get();
            return Ok(lstfolders);
        }

        [HttpGet("{id}")]
        public Base GetFolderById(int id)
        {
            Base InfoObj = new Base();
      
            InfoObj = _FoldersRepository.GetById(id);

            return InfoObj;
        }

        [HttpPost]
        public Base InsertFolder(string FolderName,string Path)
        {
            Base InfoObj = new Base();
            var InsertObj = new Folders { FolderName = FolderName, FolderPath = Path };

            InfoObj = _FoldersRepository.Insert(InsertObj);
            return InfoObj;
        }

        [HttpDelete("{id}")]
      
        //[ActionName("DeleteFolder")]
        public Base DeleteFolder(int id)
        {
            Base InfoObj = new Base();

            InfoObj = _FoldersRepository.Delete(id);
            return InfoObj;
        }

        [HttpPut]
        public Base RenameFolder(Folders obj)
        {
            Base InfoObj = new Base();

            InfoObj = _FoldersRepository.Rename(obj);
            return InfoObj;
        }

        //// GET api/<FoldersController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<FoldersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<FoldersController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<FoldersController>/5

    }
}
