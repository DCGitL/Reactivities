import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import { useLocation } from "react-router";



export  const useData = <T>(responsePath: string, id?: string | number, currentUser?: User ) => {

    const  controller = new AbortController();
    const queryClient = useQueryClient();
    const location = useLocation();   
    const {data: items, isPending} = useQuery({
      
        queryKey: [responsePath],
        queryFn: async () => {
          const { data } = await agent.get<T[]>(`/${responsePath}`, {signal: controller.signal});
          return data;
        },
       // staleTime: 1000 * 60 * 5,    //this is the amount of time the data will be cached option 1
        enabled: !id && location.pathname === `/${responsePath}` && !!currentUser //'/activities' // this is another option to only enable when path is activities
      });

    const {data: item, isLoading: isLoadingItem} = useQuery({
      queryKey: [responsePath, id],
      queryFn: async () => {
        const {data} = await agent.get<T>(`/${responsePath}/${id}`, {signal: controller.signal});
        return data;
      },
       enabled: !!id && !!currentUser
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
      mutationFn:  async (activity: T & {id: string | number}) => {
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


  