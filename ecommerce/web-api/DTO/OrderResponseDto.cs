using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommapp.Models;

namespace ecommapp.DTO
{
    public class OrderResponseDto
    {
        public Order Order { get; set; }
        public byte[] ReceiptFile { get; set; }
        public string ReceiptFileName { get; set; }

    }
}