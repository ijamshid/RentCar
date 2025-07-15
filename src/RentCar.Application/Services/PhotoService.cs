using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Application.Models.Photo;
using RentCar.Core.Entities;
using RentCar.DataAccess.Persistence;

namespace RentCar.Application.Services;

public class PhotoService(DatabaseContext context, IMapper mapper, IMemoryCache cache) : IPhotoService
{
    private readonly DatabaseContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;

    public Task<IQueryable<PhotoModel>> GetPhotosAsync()
    {
        // CacheKey
        var cacheKey = "photos";

        if (_cache.TryGetValue(cacheKey, out IQueryable<PhotoModel> cachedPhotos))
            return Task.FromResult(cachedPhotos);

        var photos = _context.Photos.AsQueryable().AsNoTracking();
        var mappedPhotos = _mapper.ProjectTo<PhotoModel>(photos);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        _cache.Set(cacheKey, mappedPhotos, cacheOptions);

        return Task.FromResult(mappedPhotos);
    }


    public async Task<PhotoModel> GetPhotoBtCarIdAsync(int id)
    {
        var cacheKey = $"photo_car_{id}";

        if (_cache.TryGetValue(cacheKey, out PhotoModel cachedPhoto))
            return cachedPhoto;

        var photo = await _context.Photos.FirstOrDefaultAsync(k => k.CarId == id);

        if (photo is null)
            throw new Exception("Not found");

        var photoDto = _mapper.Map<PhotoModel>(photo);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        _cache.Set(cacheKey, photoDto, cacheOptions);

        return photoDto;
    }

    public async Task<ResponsePhotoModel> CreatePhotoAsync(CreatePhotoModel model)
    {
        var photoModel = _mapper.Map<Photo>(model);

        await _context.AddAsync(photoModel);
        await _context.SaveChangesAsync();

        // Cache'ni yangilash
        _cache.Remove("photos");
        _cache.Remove("photos_admin");

        return new ResponsePhotoModel()
        {
            IsSuccess = true,
            Status = "Successfully created"
        };
    }

    public async Task<ResponsePhotoModel> UpdatePhotoAsync(UpdatePhotoModel model)
    {
        var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == model.Id);

        if (photo is null)
            throw new Exception("Photo not found");

        // Ma'lumotlarni yangilash
        photo.Url = model.Url;
        photo.CarId = model.CarId;

        _context.Photos.Update(photo);
        await _context.SaveChangesAsync();

        // Cache'ni tozalash
        _cache.Remove("photos");
        _cache.Remove("photos_admin");
        _cache.Remove($"photo_car_{photo.CarId}");

        return new ResponsePhotoModel()
        {
            IsSuccess = true,
            Status = "Successfully updated"
        };
    }


    public async Task<ResponsePhotoModel> DeletePhotoAsync(int id)
    {
        var photo = await _context.Photos.FirstOrDefaultAsync(key => key.Id == id);

        if (photo is null)
            throw new Exception("Not found");

        _context.Photos.Remove(photo);
        await _context.SaveChangesAsync();

        // Cache'ni tozalash
        _cache.Remove("photos");
        _cache.Remove("photos_admin");
        _cache.Remove($"photo_car_{photo.CarId}");

        return new ResponsePhotoModel()
        {
            IsSuccess = true,
            Status = "Deleted Successfully"
        };
    }
}
