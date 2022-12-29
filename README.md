# .net core 3.1 ConsoleApplication Demo


## 一、AutoFac

+ 2.Services 中命名以"service"结尾的自动以Scope注入
+ autofac配置路径，也可以根据需要自行配置：src/4.Infrastructure/ConfigureAutofac

## 二、写一个服务器

+ 每一个服务器，在2.Services下面独立建文件夹，通过环境变量SERVICE_NAME，来确认执行的业务
+ 服务实现接口 IEtlHostedService;
+ 默认SERVICE_NAME 为 IEtlHostedService的实现类名，可以通过实现Name get方法自定义

#  