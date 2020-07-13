using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodeApi;

namespace CS.NodeApi.Api
{
    public partial class BCTransactionTools
    {
        /// <summary>
        /// Создаем подключение
        /// </summary>
        /// <returns></returns>
        public static API.Client CreateContextBC(string networkIp, int port, int timeout)
        {
            return NodeAPIClient.Api.ClientFactory.CreatePublicAPIClient(networkIp, port, timeout);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transac"></param>
        /// <param name="isDelegate"></param>
        /// <param name="forExecute">Для ВЫПОЛНЕНИЯ</param>
        /// <returns></returns>
        public static Byte[] CreateByteHashByTransactionDelegation(Transaction transac, bool delegateEnable = false, bool delegateDisable = false, DateTimeOffset? dateTimeOffset=null)
        {
          

            var result = BitConverter.GetBytes(transac.Id).Take(6);
            result = result.Concat(transac.Source);
            result = result.Concat(transac.Target);
            result = result.Concat(BitConverter.GetBytes(transac.Amount.Integral));
            result = result.Concat(BitConverter.GetBytes(transac.Amount.Fraction));
            result = result.Concat(BitConverter.GetBytes(transac.Fee.Commission));
            result = result.Concat(new byte[1] { (byte)transac.Currency });
            //if (isDelegate != 0)
            //{
            //    int i = 2; // must be removed
            //    //result = result.Concat(new byte[1] { (byte)i });
            //    result = result.Concat(BitConverter.GetBytes(i));
            //}

            if (transac.SmartContract != null)
            {
                result = result.Concat(new Byte[1] { 1 });
                IEnumerable<Byte> arr1 = new byte[3] { 11, 0, 1 };
                if (transac.SmartContract.Method == null)
                {
                    arr1 = arr1.Concat(new Byte[4]);
                }
                else
                {
                    arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.Method.Length).Reverse());
                    //   arr1 = arr1.Concat(Encoding.ASCII.GetBytes(transac.SmartContract.Method));
                    arr1 = arr1.Concat(Encoding.UTF8.GetBytes(transac.SmartContract.Method));

                }

                if (transac.SmartContract.Params == null)
                {
                    arr1 = arr1.Concat(new byte[8] { 15, 0, 2, 12, 0, 0, 0, 0 });
                }
                else
                {
                    arr1 = arr1.Concat(new byte[4] { 15, 0, 2, 12 });
                    arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.Params.Count).Reverse());

                    foreach (var item in transac.SmartContract.Params)
                    {
                        if (item.__isset.v_string)
                        {
                            arr1 = arr1.Concat(new byte[3] { 11, 0, 17 });
                            arr1 = arr1.Concat(BitConverter.GetBytes(item.V_string.Length).Reverse());
                            arr1 = arr1.Concat(Encoding.UTF8.GetBytes(item.V_string));
                            arr1 = arr1.Concat(new byte[1]);
                        }
                        else if (item.__isset.v_double)
                        {
                            arr1 = arr1.Concat(new byte[3] { 4, 0, 15 });
                            arr1 = arr1.Concat(BitConverter.GetBytes(item.V_double).Reverse());
                            arr1 = arr1.Concat(new byte[1]);
                        }
                        else if (item.__isset.v_int)
                        {
                            arr1 = arr1.Concat(new byte[3] { 8, 0, 9 });
                            arr1 = arr1.Concat(BitConverter.GetBytes(item.V_int).Reverse());
                            arr1 = arr1.Concat(new byte[1]);
                        }
                        else if (item.__isset.v_boolean)
                        {

                            if (item.V_boolean)
                            {
                                arr1 = arr1.Concat(new byte[5] { 2, 0, 3, 1, 0 });
                            }
                            else
                            {
                                arr1 = arr1.Concat(new byte[5] { 2, 0, 3, 0, 0 });
                            }
                        }
                    }

                }




                if (transac.SmartContract.UsedContracts == null)
                {
                    arr1 = arr1.Concat(new byte[8] { 15, 0, 3, 11, 0, 0, 0, 0 });
                }
                else
                {
                    arr1 = arr1.Concat(new byte[4] { 15, 0, 3, 11 });
                    arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.UsedContracts.Count).Reverse());

                    foreach (var item in transac.SmartContract.UsedContracts)
                    {
                        arr1 = arr1.Concat(BitConverter.GetBytes(item.Length).Reverse());
                        arr1 = arr1.Concat(item);
                    }

                }


                if (transac.SmartContract.ForgetNewState)
                {
                    arr1 = arr1.Concat(new byte[4] { 2, 0, 4, 1 });
                }
                else
                {
                    arr1 = arr1.Concat(new byte[4] { 2, 0, 4, 0 });
                }


                if (transac.SmartContract.SmartContractDeploy != null)
                {
                    arr1 = arr1.Concat(new byte[6] { 12, 0, 5, 11, 0, 1 });

                    arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.SmartContractDeploy.SourceCode.Length).Reverse());
                    arr1 = arr1.Concat(Encoding.UTF8.GetBytes(transac.SmartContract.SmartContractDeploy.SourceCode));

                    arr1 = arr1.Concat(new byte[4] { 15, 0, 2, 12 });

                    arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.SmartContractDeploy.ByteCodeObjects.Count).Reverse());

                    foreach (var item in transac.SmartContract.SmartContractDeploy.ByteCodeObjects)
                    {
                        arr1 = arr1.Concat(new byte[3] { 11, 0, 1 });
                        arr1 = arr1.Concat(BitConverter.GetBytes(item.Name.Length).Reverse());
                        arr1 = arr1.Concat(Encoding.UTF8.GetBytes(item.Name));

                        arr1 = arr1.Concat(new byte[3] { 11, 0, 2 });
                        arr1 = arr1.Concat(BitConverter.GetBytes(item.ByteCode.Length).Reverse());
                        arr1 = arr1.Concat(item.ByteCode);
                        arr1 = arr1.Concat(new byte[1]);

                    }
                    arr1 = arr1.Concat(new byte[15] { 11, 0, 3, 0, 0, 0, 0, 8, 0, 4, 0, 0, 0, 0, 0 });
                }
                //result = result.Concat(arr1);
                //Console.WriteLine(Base58Check.Base58CheckEncoding.EncodePlain(result.ToArray()));
                //Console.WriteLine();



                arr1 = arr1.Concat(new byte[3] { 6, 0, 6 });
                arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.Version).Reverse());
                arr1 = arr1.Concat(new byte[1]);



                result = result.Concat(BitConverter.GetBytes(arr1.ToArray().Length).Take(4).ToArray());
                result = result.Concat(arr1);




            }
            //else if (isDelegate != 0)
            //{
            //    //result = result.Concat(new byte[1] { (byte)isDelegate });
            //    result = result.Concat(BitConverter.GetBytes(isDelegate));
            //}
            else if (transac.UserFields != null)// && !forExecute)
            {

                result = result.Concat(new byte[1] { 1 });
                result = result.Concat(BitConverter.GetBytes(transac.UserFields.Length));
                result = result.Concat(transac.UserFields);


            }
            else if (transac.UsedContracts != null)
            {
                result = result.Concat(new byte[1] { 1 });

                IEnumerable<Byte> arr1 = new byte[27] { 11, 0, 1, 0, 0, 0, 0, 15, 0, 2, 12, 0, 0, 0, 0, 15, 0, 3, 11, 0, 0, 0, 0, 15, 0, 3, 11 };
                arr1 = arr1.Concat(BitConverter.GetBytes(transac.UsedContracts.Count).Reverse());

                foreach (var item in transac.UsedContracts)
                {
                    arr1 = arr1.Concat(BitConverter.GetBytes(item.Length).Reverse());
                    arr1 = arr1.Concat(item);
                }

                arr1 = arr1.Concat(new byte[5] { 2, 0, 4, 0, 1 });


                result = result.Concat(BitConverter.GetBytes(arr1.ToArray().Length).Take(4));
                result.Concat(arr1);
            }
            else if (delegateDisable||delegateEnable)
            {
                result = result.Concat(new byte[1] { 1 });
                if (delegateEnable)
                {
                    result = result.Concat(new byte[8] { 1, 0, 0, 0, 0, 0, 0, 0 });
                }
                else if (delegateDisable)
                {
                    result = result.Concat(new byte[8] { 2, 0, 0, 0, 0, 0, 0, 0 });
                }else if (dateTimeOffset != null)
                {

                    //  var unixTime = Convert.ToUInt64((dateTimeOffset).ToUnixTimeSeconds());
                    //   result = result.Concat(BitConverter.GetBytes(unixTime));

                }

            }
            
            else
            {
                result = result.Concat(new Byte[1]);
                //    result = result.Concat(new byte[1] { 1 });
            }



            var res = result.ToArray();
            return res;
        }

        public static Byte[] CreateByteHashByTransaction(Transaction transac)
        {
            var result = BitConverter.GetBytes(transac.Id).Take(6);
            result = result.Concat(transac.Source);
            result = result.Concat(transac.Target);
            result = result.Concat(BitConverter.GetBytes(transac.Amount.Integral));
            result = result.Concat(BitConverter.GetBytes(transac.Amount.Fraction));
            result = result.Concat(BitConverter.GetBytes(transac.Fee.Commission));
            result = result.Concat(new byte[1] { (byte)transac.Currency });

            if (transac.SmartContract != null)
            {
                if (transac.UserFields == null)
                {
                    result = result.Concat(new Byte[1] { 1 });
                }
                else
                {
                    result = result.Concat(new Byte[1] { 2 });
                }


                IEnumerable<Byte> arr1 = new byte[3] { 11, 0, 1 };

                if (transac.SmartContract.Method == null)
                {
                    arr1 = arr1.Concat(new Byte[4]);
                }
                else
                {
                    arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.Method.Length).Reverse());
                    //   arr1 = arr1.Concat(Encoding.ASCII.GetBytes(transac.SmartContract.Method));
                    arr1 = arr1.Concat(Encoding.UTF8.GetBytes(transac.SmartContract.Method));

                }

                if (transac.SmartContract.Params == null)
                {
                    arr1 = arr1.Concat(new byte[8] { 15, 0, 2, 12, 0, 0, 0, 0 });
                }
                else
                {
                    arr1 = arr1.Concat(new byte[4] { 15, 0, 2, 12 });
                    arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.Params.Count).Reverse());

                    foreach (var item in transac.SmartContract.Params)
                    {
                        if (item.__isset.v_string)
                        {
                            arr1 = arr1.Concat(new byte[3] { 11, 0, 17 });
                            arr1 = arr1.Concat(BitConverter.GetBytes(item.V_string.Length).Reverse());
                            arr1 = arr1.Concat(Encoding.UTF8.GetBytes(item.V_string));
                            arr1 = arr1.Concat(new byte[1]);
                        }
                        else if (item.__isset.v_double)
                        {
                            arr1 = arr1.Concat(new byte[3] { 4, 0, 15 });
                            arr1 = arr1.Concat(BitConverter.GetBytes(item.V_double).Reverse());
                            arr1 = arr1.Concat(new byte[1]);
                        }
                        else if (item.__isset.v_int)
                        {
                            arr1 = arr1.Concat(new byte[3] { 8, 0, 9 });
                            arr1 = arr1.Concat(BitConverter.GetBytes(item.V_int).Reverse());
                            arr1 = arr1.Concat(new byte[1]);
                        }
                        else if (item.__isset.v_boolean)
                        {

                            if (item.V_boolean)
                            {
                                arr1 = arr1.Concat(new byte[5] { 2, 0, 3, 1, 0 });
                            }
                            else
                            {
                                arr1 = arr1.Concat(new byte[5] { 2, 0, 3, 0, 0 });
                            }
                        }
                    }

                }




                if (transac.SmartContract.UsedContracts == null)
                {
                    arr1 = arr1.Concat(new byte[8] { 15, 0, 3, 11, 0, 0, 0, 0 });
                }
                else
                {
                    arr1 = arr1.Concat(new byte[4] { 15, 0, 3, 11 });
                    arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.UsedContracts.Count).Reverse());

                    foreach (var item in transac.SmartContract.UsedContracts)
                    {
                        arr1 = arr1.Concat(BitConverter.GetBytes(item.Length).Reverse());
                        arr1 = arr1.Concat(item);
                    }

                }


                if (transac.SmartContract.ForgetNewState)
                {
                    arr1 = arr1.Concat(new byte[4] { 2, 0, 4, 1 });
                }
                else
                {
                    arr1 = arr1.Concat(new byte[4] { 2, 0, 4, 0 });
                }


                if (transac.SmartContract.SmartContractDeploy != null)
                {
                    arr1 = arr1.Concat(new byte[6] { 12, 0, 5, 11, 0, 1 });

                    arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.SmartContractDeploy.SourceCode.Length).Reverse());
                    arr1 = arr1.Concat(Encoding.UTF8.GetBytes(transac.SmartContract.SmartContractDeploy.SourceCode));

                    arr1 = arr1.Concat(new byte[4] { 15, 0, 2, 12 });

                    arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.SmartContractDeploy.ByteCodeObjects.Count).Reverse());

                    foreach (var item in transac.SmartContract.SmartContractDeploy.ByteCodeObjects)
                    {
                        arr1 = arr1.Concat(new byte[3] { 11, 0, 1 });
                        arr1 = arr1.Concat(BitConverter.GetBytes(item.Name.Length).Reverse());
                        arr1 = arr1.Concat(Encoding.UTF8.GetBytes(item.Name));

                        arr1 = arr1.Concat(new byte[3] { 11, 0, 2 });
                        arr1 = arr1.Concat(BitConverter.GetBytes(item.ByteCode.Length).Reverse());
                        arr1 = arr1.Concat(item.ByteCode);
                        arr1 = arr1.Concat(new byte[1]);

                    }
                    arr1 = arr1.Concat(new byte[15] { 11, 0, 3, 0, 0, 0, 0, 8, 0, 4, 0, 0, 0, 0, 0 });
                }
                //result = result.Concat(arr1);
                //Console.WriteLine(Base58Check.Base58CheckEncoding.EncodePlain(result.ToArray()));
                //Console.WriteLine();



                arr1 = arr1.Concat(new byte[3] { 6, 0, 6 });
                arr1 = arr1.Concat(BitConverter.GetBytes(transac.SmartContract.Version).Reverse());
                arr1 = arr1.Concat(new byte[1]);



                result = result.Concat(BitConverter.GetBytes(arr1.ToArray().Length).Take(4).ToArray());
                result = result.Concat(arr1);




            }
            else if (transac.UsedContracts != null)
            {
                result = result.Concat(new byte[1] { 1 });

                IEnumerable<Byte> arr1 = new byte[27] { 11, 0, 1, 0, 0, 0, 0, 15, 0, 2, 12, 0, 0, 0, 0, 15, 0, 3, 11, 0, 0, 0, 0, 15, 0, 3, 11 };
                arr1 = arr1.Concat(BitConverter.GetBytes(transac.UsedContracts.Count).Reverse());

                foreach (var item in transac.UsedContracts)
                {
                    arr1 = arr1.Concat(BitConverter.GetBytes(item.Length).Reverse());
                    arr1 = arr1.Concat(item);
                }

                arr1 = arr1.Concat(new byte[5] { 2, 0, 4, 0, 1 });


                result = result.Concat(BitConverter.GetBytes(arr1.ToArray().Length).Take(4));
                result.Concat(arr1);
            }
            else
            {
                if (transac.UserFields != null)
                {
                    result = result.Concat(new byte[1] { 1 });
                }
                else
                {
                    result = result.Concat(new Byte[1]);
                }
            }
            if (transac.UserFields != null)
            {
                //result = result.Concat(new byte[1] { 1 });
                result = result.Concat(BitConverter.GetBytes(transac.UserFields.Length));
                result = result.Concat(transac.UserFields);

            }

            var res = result.ToArray();
            return res;
        }

        /// <summary>
        /// Конкатенация масива байтов
        /// </summary>
        /// <param name="ar1"></param>
        /// <param name="ar2"></param>
        public static void ConcatArray(ref Byte[] ar1, Byte[] ar2)
        {
            ar1 = ar1.Concat(ar2).ToArray();
        }


        public static Amount GetAmountByDouble(double value)
        {
            Amount res = new Amount
            {
                Integral = (int)value
            };
            if (res.Integral == 0)
            {
                res.Fraction = System.Convert.ToInt64(value * Math.Pow(10, 18));
            }
            else
            {
                res.Fraction = System.Convert.ToInt64((res.Integral - value) * Math.Pow(10, 18));
            }
            return res;
        }


        public static Decimal GetDecimalByAmount(Amount value)
        {
            if(value is null)
            {
                return 0;
            }
            Decimal res = value.Integral +
                Decimal.Multiply(value.Fraction, 0.000000000000000001M);


            return res;
        }


        [Obsolete]
        public static SmartContractInvocation
            InitSmartContractInvocation()
        {
            SmartContractInvocation result = null;



            return result;
        }
    }


}
