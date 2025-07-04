using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVentas.DTO;

namespace SistemaVentas.BLL.Services.Contract
{
    public interface IRolService
    {
        Task<List<RolDTO>> List();


    }
}
