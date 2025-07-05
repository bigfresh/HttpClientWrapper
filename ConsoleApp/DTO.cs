namespace ConsoleApp;

public record RootObject(
    Args args,
    Headers headers,
    string url
);

public record Args(
    string foo1,
    string foo2
);

public record Headers(
    string x_forwarded_proto,
    string host,
    string accept,
    string accept_encoding,
    string cache_control,
    string postman_token,
    string user_agent,
    string x_forwarded_port
);