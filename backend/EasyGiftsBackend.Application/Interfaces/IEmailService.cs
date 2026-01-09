using System;
using System.Collections.Generic;
using System.Text;

namespace EasyGiftsBackend.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }
}

