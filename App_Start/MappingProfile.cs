using AutoMapper;
using StudentRegSys.Dtos;
using StudentRegSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentRegSys.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Major, MajorDto>();
            Mapper.CreateMap<MajorDto, Major>()
            .ForMember(m => m.ID, opt => opt.Ignore());

            Mapper.CreateMap<Student, StudentDto>();
            Mapper.CreateMap<StudentDto, Student>()
            .ForMember(m => m.ID, opt => opt.Ignore());
        }
    }
}