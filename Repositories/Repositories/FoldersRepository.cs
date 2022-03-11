using FileManagment.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class FoldersRepository : IFoldersRepository
    {
        private readonly ApplicationDbContext context;
        public FoldersRepository(ApplicationDbContext _context)
        {
            this.context = _context;
        }
        public List<Folders> Get()
        {
            var lstFolders = context.folders
                .Include(a=>a.file)
                .AsQueryable().ToList();
            return lstFolders;
        }

        public Base GetById(int folderId)
        {
            Base InfoObj = new Base();

            Folders objFolder = context.folders.Include (a=>a.file)
                
                .AsQueryable().FirstOrDefault(f => f.ID == folderId);
            if (objFolder != null)
            {
                InfoObj.ErrorCode = 0;
                InfoObj.ErrorDesc = "Get Folder >> Folder information exists";
                InfoObj.folder = objFolder;
                return InfoObj;
            }
            else
            {
                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "Get Folder >> Folder information not exists";
                InfoObj.folder = null;
                return InfoObj;
            }
        }

        public Base Insert(Folders objFolder)
        {
            Base InfoObj = new Base();
            try
            {
                if (context.folders.Any(a => a.FolderName == objFolder.FolderName & a.FolderPath == objFolder.FolderPath))
                {
                    InfoObj.ErrorCode = -1;
                    InfoObj.ErrorDesc = "Insert Folder >> Folder already exists";
                    InfoObj.folder = null;
                    return InfoObj;
                    //return null;
                }

                var dir = objFolder.FolderPath + "\\" + objFolder.FolderName;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                context.folders.Add(objFolder);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    File.Delete(dir);

                    InfoObj.ErrorCode = -1;
                    InfoObj.ErrorDesc = "Insert Folder >> Folder not added successfully";
                    InfoObj.folder = null;
                    return InfoObj;
                }
            }
            catch(Exception ex)
            {
                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "Insert Folder >> General error";
                InfoObj.folder = null;
                return InfoObj;
                //return null;
            }
            var folder = context.folders.AsQueryable().FirstOrDefault(f => f.FolderName == objFolder.FolderName && f.FolderPath == objFolder.FolderPath);
            //return folder;
            InfoObj.ErrorCode = 0;
            InfoObj.ErrorDesc = "Insert Folder >> Folder added successfully";
            InfoObj.folder = folder;
            return InfoObj;
        }

        public Base Delete(int folderId)
        {
            Base InfoObj = new Base();
            try
            {
                Folders obj = context.folders.AsQueryable().FirstOrDefault(f => f.ID == folderId);
                if (obj!= null)
                {
                    if (context.files.AsQueryable().Any(a => a.FoldersID == folderId))
                    {
                        var lstFiles = context.files.AsQueryable().Where(w => w.FoldersID == folderId).ToList();
                        foreach (var item in lstFiles)
                        {
                            context.files.Remove(item);
                        }
                        context.SaveChanges();
                    }
                    var dir = obj.FolderPath + "/" + obj.FolderName;

                    if (Directory.Exists(dir))
                        Directory.Delete(dir,true);
                    else
                    {
                        InfoObj.ErrorCode = -1;
                        InfoObj.ErrorDesc = "Delete Folder >> Folder path not physical exists";
                        InfoObj.folder = obj;
                        return InfoObj;
                    }

                    context.folders.Remove(obj);
                    context.SaveChanges();
                }
                else
                {
                    InfoObj.ErrorCode = -1;
                    InfoObj.ErrorDesc = "Delete Folder >> Folder id not exists";
                    InfoObj.folder = null;
                    return InfoObj;
                }

                InfoObj.ErrorCode = 0;
                InfoObj.ErrorDesc = "Delete Folder >> Path deleted successfully";
                InfoObj.folder = obj;
                return InfoObj;
            }
            catch (Exception ex)
            {
                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "Delete Folder >> General error";
                InfoObj.folder = null;
                return InfoObj;
            } 
        }

        public Base Rename(Folders objFolder)
        {
            Base InfoObj = new Base();

            Folders obj = context.folders.AsQueryable().FirstOrDefault(f => f.ID == objFolder.ID);
            var OldDir = obj.FolderPath + "\\" + obj.FolderName;
            var NewDir = objFolder.FolderPath + "\\" + objFolder.FolderName;
            if (!Directory.Exists(OldDir))
            {
                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "Rename Folder >> Folder not physical exists";
                InfoObj.folder = null;
                return InfoObj;
            }
            else
            {
                if (context.folders.Any(a => a.FolderName == objFolder.FolderName & a.FolderPath == objFolder.FolderPath))
                {
                    InfoObj.ErrorCode = -1;
                    InfoObj.ErrorDesc = "Rename Folder >> New folder already exists";
                    InfoObj.folder = null;
                    return InfoObj;
                }

                obj.FolderName = objFolder.FolderName;
                obj.FolderPath = objFolder.FolderPath;
                context.folders.Update(obj);
                context.SaveChanges();

                Directory.Move(OldDir, NewDir);
            }
            var folder = context.folders.AsQueryable().FirstOrDefault(f => f.ID== objFolder.ID);
            InfoObj.ErrorCode = 0;
            InfoObj.ErrorDesc = "Rename Folder >> Rename folder done successfully";
            InfoObj.folder = folder;
            return InfoObj;
           
        }
    }
}
