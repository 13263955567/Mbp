using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mbp.Core.Core;
using Mbp.AspNetCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Mbp.Authentication;

namespace Medical.Ai.Mbdp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // DI---->IOC
        public void ConfigureServices(IServiceCollection services)
        {
            // ����ҽѧ������ƽ̨���
            services.AddMedicalFramework<AspMbpModuleManager>();
        }

        // asp.net core �ܵ�,�ܵ�˳���΢�������˵����Щ��ͻ,������Դ�����м����Ҫ��Routing��EndPoints֮��,������ֻ������EndPoints֮ǰ
        // �������������ھ���,��ʱ�����������м��˳��.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ʹ��ҽѧ�����ݿ���ƽ̨���
            app.UseMedicalFramework();

            // ·���м��
            app.UseRouting();

            // �����֤�м��
            app.UseAuthorization();

            // ·���ս������ �����ս��֮��,mbp��Ȩ�޹����������м������ʽ��������,��������ӵ�ActionDescriptor 
            // Ҳ����˵,���ǲ�Ҫѡ�������ַ�ʽ�����غ��Զ����Ȩ�㷨
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
