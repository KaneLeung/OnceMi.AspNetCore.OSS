using FreeRedis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using OnceMi.AspNetCore.OSS;
using Sample.AspNetCore.Mvc.CacheProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.AspNetCore.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ʹ��Redis���滻Ĭ�ϵĻ���ʵ��
            var client = new RedisClient("127.0.0.1:6379,password=,ConnectTimeout=3000,defaultdatabase=0");
            services.TryAddSingleton<RedisClient>(client);
            services.TryAddSingleton<ICacheProvider, RedisCacheProvider>();

            //default minio
            //���Ĭ�϶��󴢴�������Ϣ
            services.AddOSSService(option =>
            {
                option.Provider = OSSProvider.Minio;
                option.Endpoint = "oss.oncemi.com:9000";  //����Ҫ����Э��
                option.AccessKey = "root";
                option.SecretKey = "Q*************************f";
                option.IsEnableHttps = true;
                option.IsEnableCache = true;
            });

            //aliyun oss
            //�������Ϊ��aliyunoss����OSS���󴢴�������Ϣ
            services.AddOSSService("aliyunoss", option =>
            {
                option.Provider = OSSProvider.Aliyun;
                option.Endpoint = "oss-cn-hangzhou.aliyuncs.com";
                option.AccessKey = "L*********************U";
                option.SecretKey = "D**************************M";
                option.IsEnableHttps = true;
                option.IsEnableCache = true;
            });

            //qcloud oss
            //�������ļ��м��ؽڵ�Ϊ��OSSProvider����������Ϣ
            services.AddOSSService("QCloud", "OSSProvider");

            //qiniu oss
            //�������Ϊ��qiuniu����OSS���󴢴�������Ϣ
            services.AddOSSService("qiuniu", option =>
            {
                option.Provider = OSSProvider.Qiniu;
                option.Region = "CN_East";  //֧�ֵ�ֵ��CN_East(����)/CN_South(����)/CN_North(����)/US_North(����)/Asia_South(������)
                option.AccessKey = "B****************************L";
                option.SecretKey = "Z*************************************g";
                option.IsEnableHttps = true;
                option.IsEnableCache = true;
            });

            //��Ϊ��OBS
            //�������Ϊ��huaweiobs����OSS���󴢴�������Ϣ
            //Endpoint��ѯ��https://developer.huaweicloud.com/endpoint?OBS
            services.AddOSSService("huaweiobs", option =>
            {
                option.Provider = OSSProvider.HuaweiCloud;
                option.Endpoint = "obs.cn-southwest-2.myhuaweicloud.com"; //����Ҫ����Э��
                option.Region = "cn-southwest-2";
                option.AccessKey = "R********************6";
                option.SecretKey = "5*************************************c";
                option.IsEnableHttps = true;
                option.IsEnableCache = true;
            });

            //�ٶ���BOS
            //�������Ϊ��baidubos����OSS���󴢴�������Ϣ
            //Endpoint��ѯ��https://developer.huaweicloud.com/endpoint?OBS
            services.AddOSSService("baidubos", option =>
            {
                option.Provider = OSSProvider.BaiduCloud;
                option.Endpoint = "https://su.bcebos.com"; //��Ҫ����Э��
                option.AccessKey = "A********************O";
                option.SecretKey = "d********************d";
                option.IsEnableHttps = true;
                option.IsEnableCache = true;
            });

            //������OOS
            //�������Ϊ��ctyunoos����OSS���󴢴�������Ϣ
            //Endpoint��ѯ��https://www.ctyun.cn/document/10026693/10027878
            services.AddOSSService("ctyunoos", option =>
            {
                option.Provider = OSSProvider.Ctyun;
                option.Endpoint = "oos-sdqd.ctyunapi.cn"; //����Ҫ����Э��
                option.AccessKey = "6********************6";
                option.SecretKey = "c********************5";
                option.IsEnableHttps = true;
                option.IsEnableCache = true;
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
