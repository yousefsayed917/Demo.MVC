using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;

namespace Demo.PL.MappingProfiles
{
    public class EmployeeProfile :Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap()/*.ForMember(a=>a.Name,b=>b.MapFrom(c=>c.EmpName)) دا كدا لو غيرت اسم اي بروبيرتي ف الفيو */;
        }
    }
}
