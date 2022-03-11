using FileManagment.Model;
using System;

namespace Repositories
{
    public class Base
    {
        public int ErrorCode { get; set; }
        public string ErrorDesc{ get; set; }
        public Folders folder{ get; set; }
    }

    public class FileBase
    {
        public int ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        public Files file { get; set; }
    }
}
