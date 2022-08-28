﻿// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable InconsistentNaming

namespace TestProject1 {

    public static class IOHelper {

        public static string MapPath(string virtualPath) {
            string path = Path.GetDirectoryName(typeof(IOHelper).Assembly.Location)!;
            if (virtualPath.StartsWith("~/")) return Path.Combine(path, virtualPath.Substring(2));
            throw new Exception("NÆH!");
        }

    }

}