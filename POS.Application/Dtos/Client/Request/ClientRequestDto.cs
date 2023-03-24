﻿namespace POS.Application.Dtos.Client.Request
{
    public class ClientRequestDto
    {
        public string? Name { get; set; }
        public int DocumentTypeId { get; set; }
        public string? DocumentNumber { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int State { get; set; }
    }
}
