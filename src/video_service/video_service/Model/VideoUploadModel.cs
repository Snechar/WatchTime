﻿using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace video_service.Model
{
    public class VideoUploadModel 
    {

        public string Title { get; set; }

        public string Key { get; set; }

        public IFormFile file { get; set; }

    }
}
