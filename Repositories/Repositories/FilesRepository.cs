using ExcelDataReader;
using FileManagment.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace Repositories
{
    public class FilesRepository : IFilesRepository
    {
        private readonly ApplicationDbContext context;
        public FilesRepository(ApplicationDbContext _context)
        {
            this.context = _context;
        }

        public List<Folders> GetAllFiles(int folderId)
        {
            var lstfiles = context.folders
                .Include(a => a.file)
                .AsQueryable().Where(w=>w.ID==folderId).ToList();
            return lstfiles;
        }
        public FileBase GetFileById(int fileId)
        {
            FileBase InfoObj = new FileBase();

            Files objFiles = context.files.AsQueryable().FirstOrDefault(f => f.ID == fileId);

            if (objFiles != null)
            {
                InfoObj.ErrorCode = 0;
                InfoObj.ErrorDesc = "Get File >> File information exists";
                InfoObj.file = objFiles;
                return InfoObj;
            }
            else
            {
                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "Get File >> File information not exists";
                InfoObj.file = null;
                return InfoObj;
            }
        }

        public FileBase Insert(int folderId, IFormFile obj)
        {
            FileBase InfoObj = new FileBase();
            string fileName = obj.FileName;

            var folder = context.folders.AsQueryable().FirstOrDefault(f => f.ID == folderId);
            var folderdir = folder.FolderPath + "\\" + folder.FolderName;
            string OrgFilepath = Path.Combine(folderdir + "\\" + fileName);

            if (File.Exists(OrgFilepath))
            {
                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "Upload File >> File already exists";
                InfoObj.file = null;
                return InfoObj;
            }

            var LtsStrFile = ReadAsListCamp(obj);
            var FileBytes = LtsStrFile.SelectMany(s => System.Text.Encoding.UTF8.GetBytes(s + Environment.NewLine)).ToArray();

            using (FileStream filestream = System.IO.File.Create(OrgFilepath))
            {
                obj.CopyTo(filestream);
                filestream.Flush();
            }

            Files objFile = new Files { FileName = fileName,FileData= FileBytes, FoldersID = folderId };
            context.files.Add(objFile);
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                File.Delete(OrgFilepath);

                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "Upload File >> File not added successfully";
                InfoObj.file = null;
                return InfoObj;
            }

            InfoObj.ErrorCode = 0;
            InfoObj.ErrorDesc = "Upload File >> File added successfully";
            InfoObj.file = context.files.AsQueryable().FirstOrDefault(f => f.FileName == objFile.FileName && f.FoldersID == folderId);

            return InfoObj;
        
        }
        public FileBase Delete(int fileId)
        {
            FileBase InfoObj = new FileBase();
            try
            {
                Files obj = context.files.AsQueryable().FirstOrDefault(f => f.ID == fileId);
               // var FileData = System.Text.Encoding.UTF8.GetString(obj.FileData);
                if (obj != null)
                {
                    Folders fileFolder = context.folders.AsQueryable().FirstOrDefault(f=>f.ID==obj.FoldersID);
                    var dir = fileFolder.FolderPath + "//" + fileFolder.FolderName;

                    File.Delete(Path.Combine(dir, obj.FileName));

                    context.files.Remove(obj);
                    context.SaveChanges();
                }
                else
                {
                    InfoObj.ErrorCode = -1;
                    InfoObj.ErrorDesc = "Delete file >> file id not exists";
                    InfoObj.file = null;
                    return InfoObj;
                }

                InfoObj.ErrorCode = 0;
                InfoObj.ErrorDesc = "Delete file >> file deleted successfully";
                InfoObj.file = obj;
                return InfoObj;
            }
            catch (Exception ex)
            {
                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "Delete file >> General error";
                InfoObj.file = null;
                return InfoObj;
            }
        }

        public FileBase Rename(int fileId, string name)
        {
            FileBase InfoObj = new FileBase();

            var file = context.files.AsQueryable().FirstOrDefault(f => f.ID == fileId);
            var folder = context.folders.AsQueryable().FirstOrDefault(f => f.ID == file.FoldersID);
            var folderdir = folder.FolderPath + "\\" + folder.FolderName;
            string OrgFilepath = Path.Combine(folderdir + "\\" + file.FileName);
            var fileExtension = Path.GetExtension(OrgFilepath);
            name = name + fileExtension;

            if (File.Exists(folderdir + "\\" + name))
            {
                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "rename File >> File name already exists";
                InfoObj.file = null;
                return InfoObj;
            }

            File.Move(OrgFilepath, folderdir + "\\" + name);
            try
            {
                file.FileName = name;

                context.files.Update(file);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                File.Move( folderdir + "\\" + name, OrgFilepath);
                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "rename File >> error while rename process";
                InfoObj.file = null;
                return InfoObj;
            }
            InfoObj.ErrorCode = 0;
            InfoObj.ErrorDesc = "rename File >> rename done successfully";
            InfoObj.file = file;
            return InfoObj;
        }

        public FileBase ReplaceFile(int folderId, IFormFile obj)
        {
            FileBase InfoObj = new FileBase();
            string fileName = obj.FileName;
            var file = context.files.AsQueryable().FirstOrDefault(f=>f.FoldersID==folderId && f.FileName== fileName);
            var OrFileData = file.FileData;
            var folder = context.folders.AsQueryable().FirstOrDefault(f => f.ID == folderId);
            var folderdir = folder.FolderPath + "\\" + folder.FolderName;
            string OrgFilepath = Path.Combine(folderdir + "\\" + fileName);

            var LtsStrFile = ReadAsListCamp(obj);
            var FileBytes = LtsStrFile.SelectMany(s => System.Text.Encoding.UTF8.GetBytes(s + Environment.NewLine)).ToArray();

            if (!File.Exists(OrgFilepath))
            {

                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "Replace File >> File not exist";
                InfoObj.file = null;
                return InfoObj;
            }

            File.Delete(OrgFilepath);

            using (FileStream filestream = System.IO.File.Create(OrgFilepath))
            {
                obj.CopyTo(filestream);
                filestream.Flush();
            }

            file.FileData = FileBytes;
            context.files.Update(file);
            try
            {
                context.SaveChanges();
            }
            catch
            {
                File.WriteAllText(OrgFilepath, System.Text.Encoding.UTF8.GetString(OrFileData));
              
                InfoObj.ErrorCode = -1;
                InfoObj.ErrorDesc = "Replace File >> error while replace";
                InfoObj.file = null;
                return InfoObj;
            }

            InfoObj.ErrorCode = 0;
            InfoObj.ErrorDesc = "Replace File >> Replace done successfully";
            InfoObj.file = file;

            return InfoObj;
        }

        public static List<string> ReadAsListCamp(IFormFile file)
        {
            List<string> result = new List<string>();
            if (Path.GetExtension(file.FileName).ToLower() == ".txt" || Path.GetExtension(file.FileName).ToLower() == ".out")
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                            result.Add(line);
                    }
                }
                result = result.Distinct().ToList();
            }
            else
            {
                if (Path.GetExtension(file.FileName).ToLower() == ".xlsx" || Path.GetExtension(file.FileName).ToLower() == ".xls")
                {
                    using (var reader = ExcelReaderFactory.CreateReader(file.OpenReadStream()))
                    {
                        while (reader.Read())
                        {
                            string row = "";
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (reader.IsDBNull(i))
                                {
                                    row += "\t";
                                    continue;
                                }

                                if (reader.GetValue(i).ToString().Contains("\n\r") || reader.GetValue(i).ToString().Contains("\r") || reader.GetValue(i).ToString().Contains("\n"))
                                    row += "\"" + reader.GetValue(i).ToString().Trim() + "\"\t";
                                else
                                    row += reader.GetValue(i).ToString().Trim() + "\t";
                            }
                            row = row.TrimEnd('\t');

                            if (!string.IsNullOrEmpty(row))
                            {
                                result.Add(row);
                            }
                        }
                    }
                    result = result.Distinct().ToList();
                }
            }

            return result;
        }
    }
    
}
