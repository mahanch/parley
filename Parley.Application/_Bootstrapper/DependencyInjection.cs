using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Parley.Application._Bootstrapper;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            // cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzk2MDgzMjAwIiwiaWF0IjoiMTc2NDU5NTE2OSIsImFjY291bnRfaWQiOiIwMTlhZGExMDA0MzA3M2Y5ODhhNWY0MmJmZDdlYTE3OCIsImN1c3RvbWVyX2lkIjoiY3RtXzAxa2JkMTJ4eXNnMHB6MjR0YnhqeXA0a2p3Iiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.Gl0FB9XoQMJQgzdCh9gplRirYvmkDJYXD8tfLiWkRAeQJ2zlFdoHw7btxGrUVisI_ZfSxK2kkUB_LLty2eArbeZb5Ja1_xexgzTv3CJ4TnacT0FoVOc8eLeGCbJmmXtSt6CW89XwzvV_taiSSkcdsnATNTH0MEBLqCkKw4FMpSXqrPxCgakXB-pcyeqeTO9a0DD5XjBVITGaslUUFgnBGirsLdHRgL9AYVty3EzWTBH9WBcc4dHyZ5YUDMzsIab5elc-pmOLCXAJviamG9Afsaq4N88IMjsYvq6ihw_EAsmQH1K8WNunFMN8VjwO5csHmgKJ6wajONH7kUaMWNxRrQ";
        });

        return services;
    }
}