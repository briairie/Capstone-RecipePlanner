﻿using AutoMapper;
using static RecipePlannerApi.Dao.UserDao;
using System.Data.SqlClient;
using System.Data;
using AutoMapper.Data;
using RecipePlannerApi.Model;
using RecipePlannerApi.Dao.Request;

namespace RecipePlannerApi.Dao {
    public abstract class Dao {
        public static List<T> execute<T>(string storedProc, CommandUpdate updateCmd) {

            var mapper = new MapperConfiguration(config => {
                config.AddDataReaderMapping();
                config.CreateMap<IDataReader, User>();
                config.CreateMap<IDataReader, int>();
                config.CreateMap<IDataReader, IdDto>();
                config.CreateMap<IDataReader, PantryItem>();
            }).CreateMapper();

            using (SqlConnection conn = new SqlConnection(Connection.ConnectionString)) {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(storedProc, conn)) {
                    updateCmd(cmd);
                    using (var reader = cmd.ExecuteReader()) {
                        if (reader.HasRows) {
                            return mapper.Map<IDataReader, List<T>>(reader);
                        }
                    }
                }
            }

            return new List<T>();
        }
    }
}