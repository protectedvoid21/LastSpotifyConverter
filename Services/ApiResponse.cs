using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services; 

public class ApiResponse {
    public bool IsSuccess { get; private set; }
    public string Content { get; private set; }

    public ApiResponse(bool isSuccess, string content) {
        IsSuccess = isSuccess;
        Content = content;
    }
}
