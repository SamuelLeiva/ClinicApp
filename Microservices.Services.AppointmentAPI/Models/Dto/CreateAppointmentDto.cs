﻿namespace Microservices.Services.AppointmentAPI.Models.Dto
{
    public class CreateAppointmentDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Description { get; set; }
    }
}
