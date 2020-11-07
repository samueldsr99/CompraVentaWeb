using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.Utils
{
    public static class FileProcess
    {
        public static string UploadFile(IFormFile file, IHostingEnvironment hostingEnvironment)
        {
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fs);
            }

            return uniqueFileName;
        }

        public static int DeleteFile(string file, IHostingEnvironment hostingEnvironment)
        {
            string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", file);

            if (!File.Exists(filePath))
            {
                return -1;
            }

            System.IO.File.Delete(filePath);
            return 0;
        }
    }
}
