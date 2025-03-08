using AutoMapper;
using DomainLayer.Models.DataModels.OrderManagementModels;
using DomainLayer.Wrappers.DTO.OrderManagementDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DomainLayer.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderDetails, OrderDetailsResponseDTO>();
            CreateMap<OrderedItemsDetails, OrderedItemDetailsResponseDTO>();
        }
    }
}
