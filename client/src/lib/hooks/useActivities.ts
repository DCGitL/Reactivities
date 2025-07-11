

import { useAccount } from "./useAccount";
import { useData } from "./useData";

export  const useActivities = (id?: string | number) => {
   const {currentUser} = useAccount();
   const { 
          items,
          item,
          isLoadingItem, 
          isPending,
          updateData,
            createData,
            deleteData } = useData<Activity>('activities', id, currentUser);

  return {
          activities : items,  
          activity : item,
          isPending: isPending,
          isLoadingAcivity : isLoadingItem,
          updateActivity : updateData, 
          createActivity : createData, 
          deleteActivity :deleteData
      };
   
  };

  