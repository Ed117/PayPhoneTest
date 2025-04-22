using AutoMapper;
using WalletTransfer.Application.DTOs;
using WalletTransfer.Domain.Entities;

namespace WalletTransfer.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Wallet, WalletDto>().ReverseMap();
            CreateMap<Wallet, CreateWalletDto>()
                .ForMember(dest => dest.InitialBalance, opt => opt.MapFrom(src => src.Balance));
            CreateMap<UpdateWalletDto, Wallet>();
            CreateMap<TransactionHistory, TransactionHistoryDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));
        }
    }
}
