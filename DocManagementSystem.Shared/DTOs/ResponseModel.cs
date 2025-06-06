﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Shared.DTOs
{
    public class ResponseModel<T>
    {
        public bool success {  get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
