using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hospital.Core.DTO.Accountant;
using Hospital.Core.DTO.Appointment;
using Hospital.Core.DTO.Billing;
using Hospital.Core.DTO.Department;
using Hospital.Core.DTO.Doctor;
using Hospital.Core.DTO.Inpatient_Admission;
using Hospital.Core.DTO.Laboratory;
using Hospital.Core.DTO.Patient;
using Hospital.Core.DTO.Room;
using Hospital.Core.DTO.Staff;
using Hospital.Core.Models;

namespace Hospital.Core.Helper
{
    public class HelperMapper:Profile
    {
                     
        //CreateMap<//src, //dest>();
        public HelperMapper()
        {
            // Patient
            CreateMap<Patient,CreatePatientDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)).ReverseMap();
            CreateMap<Patient, AllPatientDTO>().ReverseMap();

            //Department
            CreateMap<Department, CreateDepartmentDTO>().ReverseMap();
            CreateMap<Department, AllDepartmentDTO>().ReverseMap();

            // Doctor
            CreateMap<Doctor, CreateDoctorDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)).ReverseMap();
            CreateMap<Doctor, AllDoctorDTO>().ReverseMap();
            CreateMap<Doctor, DoctorDepartmentDTO>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name)).ReverseMap();

            // staff
            CreateMap<Staff, CreatestaffDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)).ReverseMap();
            CreateMap<Staff, AllstaffDTO>().ReverseMap();

            // Room
            CreateMap<Room, CreateRoomDTO>().ReverseMap();
            CreateMap<Room, AllRoomDTO>().ReverseMap();

            // Appointment
            CreateMap<Appointment, CreateAppointmentDTO>().ReverseMap();
            CreateMap<Appointment,AllAppointmentDTO>().ReverseMap();

            // Laboratory
            CreateMap<Laboratory, CreateLaboratoryDTO>()
            .ForMember(dest=>dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId)).ReverseMap();
            CreateMap<Laboratory, AllLaboratoryDTO>().ReverseMap();

            // Inpatient Admission
            CreateMap<Inpatient_Admission, CreateInpatient_AdmissionDTO>()
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
            .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
            .ReverseMap();

            CreateMap<Inpatient_Admission, AllInpatient_AdmissionDTO>().ReverseMap();

            // Accountant
            CreateMap<Accountant, CreateAccountantDTO>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)).ReverseMap();
            CreateMap<Accountant, AllAccountantDTO>().ReverseMap();

            // Billing
            CreateMap<Billing, CreateBillingDTO>().ReverseMap()
           .ForMember(dest => dest.AccountantId, opt => opt.MapFrom(src => src.AccountantId));
            CreateMap<Billing, AllBillingDTO>().ReverseMap();
        }
    }
}
