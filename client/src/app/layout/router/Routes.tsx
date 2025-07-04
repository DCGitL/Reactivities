import { createBrowserRouter, Navigate } from "react-router";
import App from "../App";
import HomePage from "../../../features/home/HomePage";
import ActivityDashboard from "../../../features/activities/dashboard/ActivityDashboard";
import ActivityForm from "../../../features/activities/form/ActivityForm";
import ActivityDetailPage from "../../../features/activities/details/ActivityDetailPage";
import Counter from "../../../features/counter/Counter";
import TestErrors from "../../../features/home/errors/TestErrors";
import NotFound from "../../../features/home/errors/NotFound";
import ServerError from "../../../features/home/errors/ServerError";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App/>,
        children: [
            {path: '', element: <HomePage/>},
            {path: 'activities', element: <ActivityDashboard/>},        
            {path: 'activities/:id', element: <ActivityDetailPage/>},
            {path: 'createactivity', element: <ActivityForm key='create'/>},
            {path: 'manage/:id', element: <ActivityForm/>},
            {path: 'counter', element: <Counter/>},
            {path: 'errors', element: <TestErrors/>},
            {path: 'not-found', element: <NotFound/>},
            {path: 'server-error', element: <ServerError/>},
            {path: '*', element: <Navigate replace to='/not-found'/>},


        ]
    }
])