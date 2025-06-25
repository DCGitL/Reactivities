import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";




export  const useData = <T>(responsePath: string) => {
    
    const queryClient = useQueryClient();

    const {data: data, isPending} = useQuery({
        queryKey: [responsePath],
        queryFn: async () => {
          const { data } = await agent.get<T[]>(`/${responsePath}`);
          return data;
        }});
            
    const updateData = useMutation({
      mutationFn:  async (activity: T & {id: string | number}) => {
        await agent.put(`/${responsePath}/${activity.id}`, activity);
       },
       onSuccess: async () =>{
        await queryClient.invalidateQueries({
          queryKey: [responsePath]
        });
       }
 
    });

    const createData = useMutation({
      mutationFn:  async (activity: T) => {
        await agent.post(`/${responsePath}`, activity);
       },
       onSuccess: async () =>{
        await queryClient.invalidateQueries({
          queryKey: [responsePath]
        });
       }
 
    });
    const deleteData = useMutation({
        mutationFn:  async (id: string | number) => {
          await agent.delete(`/${responsePath}/${id}`,);
         },
         onSuccess: async () =>{
          await queryClient.invalidateQueries({
            queryKey: [responsePath]
          });
         }
   
      });


    return { data, isPending, updateData, createData, deleteData};
  };

  