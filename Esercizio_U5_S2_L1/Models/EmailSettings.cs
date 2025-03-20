﻿public class EmailSettings {
    public required string From {
        get; set;
    }
    public required string SmtpServer {
        get; set;
    }
    public required int Port {
        get; set;
    }
    public required string UserName {
        get; set;
    }
    public required string Password {
        get; set;
    }
}