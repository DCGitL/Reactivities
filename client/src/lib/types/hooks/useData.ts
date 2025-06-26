import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";



export  const useData = <T>(responsePath: string, id?: string | number) => {

    const  controller = new AbortController();
    const queryClient = useQueryClient();
       
    const {data: items, isPending} = useQuery({
      
        queryKey: [responsePath],
        queryFn: async () => {
          const { data } = await agent.get<T[]>(`/${responsePath}`, {signal: controller.signal});
          return data;
        }     
      });

    const {data: item, isLoading: isLoadingItem} = useQuery({
      queryKey: [responsePath, id],
      queryFn: async () => {
        const {data} = await agent.get<T>(`/${responsePath}/${id}`, {signal: controller.signal});
        return data;
      },
       enabled: !!id
    });
            
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
       const {data} =  await agent.post(`/${responsePath}`, activity);
       return data;
       },
       onSuccess: async () =>{
        await queryClient.invalidateQueries({
          queryKey: [responsePath]
        },
      );
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


    return   { 
              items, 
              item, 
              isLoadingItem,
              isPending,
              updateData,
              createData,
              deleteData}
  };


  