using static QlHui.App.Data.Constant.Const;

namespace QlHui.App.Data.Utils
{
    internal interface IUtils
    {
        TDto TransformToDto<TDto, TEntity>(TEntity input);
        IEnumerable<TDto> TransformToDto<TDto, TEntity>(IEnumerable<TEntity> input);
        TEntity TransformToEntity<TDto, TEntity>(TDto input);
        IEnumerable<TEntity> TransformToEntity<TDto, TEntity>(IEnumerable<TDto> input);
    }
    internal class QLUtils : IUtils
    {
        public TDto TransformToDto<TDto, TEntity>(TEntity input)
        {
            if (input != null)
            {
                var dto = Activator.CreateInstance<TDto>();
                var dtoProperties = dto.GetType().GetProperties();
                var entityProperties = input.GetType().GetProperties();
                foreach (var entityProp in entityProperties)
                {
                    var entityValue = entityProp.GetValue(input);
                    if (entityValue != null)
                    {
                        var existedDto = dtoProperties.FirstOrDefault(p => p.Name == entityProp.Name);
                        if (existedDto != null)
                        {
                            if (existedDto.PropertyType == typeof(DateTime?) && entityProp.PropertyType == typeof(string))
                            {
                                DateTime dateValue = DateTime.ParseExact(entityValue.ToString(), DateTimeFormat.UIDateFormat, null);
                                existedDto.SetValue(dto, dateValue);
                            }
                            else
                            {
                                existedDto.SetValue(dto, entityValue);
                            }
                        }
                    }
                }
                return dto;
            }
            return default;
        }

        public IEnumerable<TDto> TransformToDto<TDto, TEntity>(IEnumerable<TEntity> input)
        {
            var returnData = Enumerable.Empty<TDto>();
            if (input != null)
            {
                returnData = input.Select(TransformToDto<TDto, TEntity>);
            }
            return returnData;
        }

        public TEntity TransformToEntity<TDto, TEntity>(TDto input)
        {
            if (input != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                var entityProperties = entity.GetType().GetProperties();
                var dtoProperties = input.GetType().GetProperties();
                foreach (var dtoProp in dtoProperties)
                {
                    var dtoValue = dtoProp.GetValue(input);
                    if (dtoValue != null)
                    {
                        var existedEntity = entityProperties.FirstOrDefault(p => p.Name == dtoProp.Name);
                        if (existedEntity != null)
                        {
                            if (dtoProp.PropertyType == typeof(DateTime?) && existedEntity.PropertyType == typeof(string))
                            {
                                existedEntity.SetValue(entity, ((DateTime?)dtoValue).Value.ToString(DateTimeFormat.UIDateFormat));
                            }
                            else
                            {
                                existedEntity.SetValue(entity, dtoValue);
                            }
                        }
                    }
                }
                return entity;
            }
            return default;
        }

        public IEnumerable<TEntity> TransformToEntity<TDto, TEntity>(IEnumerable<TDto> input)
        {
            var returnData = Enumerable.Empty<TEntity>();
            if (input != null)
            {
                returnData = input.Select(TransformToEntity<TDto, TEntity>);
            }
            return returnData;
        }
    }
}
