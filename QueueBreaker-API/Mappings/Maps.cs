using AutoMapper;
using QueueBreaker_API.Data;
using QueueBreaker_API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueBreaker_API.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<User, UsersDTO>().ReverseMap();
            CreateMap<User, UsersCreateDTO>().ReverseMap();
            CreateMap<User, UsersUpdateDTO>().ReverseMap();

            CreateMap<Role, RolesDTO>().ReverseMap();
            CreateMap<Role, RolesCreateDTO>().ReverseMap();
            CreateMap<Role, RolesUpdateDTO>().ReverseMap();

            CreateMap<Canteen, CanteenDTO>().ReverseMap();
            CreateMap<Canteen, CanteenCreateDTO>().ReverseMap();
            CreateMap<Canteen, CanteenUpdateDTO>().ReverseMap();

            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<Category, CategoryUpdateDTO>().ReverseMap();

            CreateMap<Item, ItemsDTO>().ReverseMap();
            CreateMap<Item, ItemsCreateDTO>().ReverseMap();
            CreateMap<Item, ItemsUpdateDTO>().ReverseMap();

            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Order, OrderCreateDTO>().ReverseMap();
            CreateMap<Order, OrderUpdateDTO>().ReverseMap();

            CreateMap<OrderItem, OrderItemsDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemsCreateDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemsUpdateDTO>().ReverseMap();

            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<Cart, CartCreateDTO>().ReverseMap();
            CreateMap<Cart, CartUpdateDTO>().ReverseMap();
        }
    }
}
