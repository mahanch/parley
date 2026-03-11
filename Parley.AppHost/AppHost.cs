using Projects;

var builder = DistributedApplication.CreateBuilder(args);



// تنظیم registry خودت
var postgres = builder.AddPostgres("postgres")
    .WithImageRegistry("docker.arvancloud.ir")
    .WithPgAdmin(c => c.WithImageRegistry("docker.arvancloud.ir"));

var parleyDb = postgres.AddDatabase("parleydb");

// تعریف Redis (اختیاری - چون گفتی فعلاً نداری)
 // var redis = builder.AddRedis("redis");

// تعریف API و وابستگی‌هاش
var api = builder.AddProject<Parley_Api>("api")
    .WithReference(parleyDb);
// .WithReference(redis); // وقتی Redis اضافه کردی uncomment کن

builder.Build().Run();