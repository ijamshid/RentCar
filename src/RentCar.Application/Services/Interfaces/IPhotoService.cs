using RentCar.Application.Models.Photo;

namespace RentCar.Application.Services;

public interface IPhotoService
{
    Task<IQueryable<PhotoModel>> GetPhotosAsync();
    Task<PhotoModel> GetPhotoBtCarIdAsync(int id);
    Task<ResponsePhotoModel> CreatePhotoAsync(CreatePhotoModel  model);
    Task<ResponsePhotoModel> UpdatePhotoAsync(UpdatePhotoModel model);
    Task<ResponsePhotoModel> DeletePhotoAsync(int id);
}