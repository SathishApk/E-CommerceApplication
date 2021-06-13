using AutoMapper;
using CommerceApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceApplication
{
    public class Automap : Profile
    {
        public Automap()
        {
            CreateMap<Product, Product>()
                .ForMember(dest=>dest.ProductImage,
                x=>x.MapFrom(src => string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(src.ProductImageData))));
        }
    }
}
