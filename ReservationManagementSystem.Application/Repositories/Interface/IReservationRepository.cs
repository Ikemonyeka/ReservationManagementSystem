using ReservationManagementSystem.Core.Entities;
using ReservationManagementSystem.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Repositories.Interface
{
    public interface IReservationRepository
    {
        Task<ResponseViewModel> NewReservation(Reservation reservation);
        Task<List<Reservation>> GetReservationByTableId(int Id, DateTime ReservationDate, string SqlConn);
    }
}
