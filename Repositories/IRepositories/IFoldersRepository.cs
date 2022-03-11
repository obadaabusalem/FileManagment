using FileManagment.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public interface IFoldersRepository
    {
        public List<Folders> Get();

        public Base GetById(int folderId);
        public Base Insert(Folders objFolder);
        public Base Delete(int folderId);
        public Base Rename(Folders objFolder);
    }
}
