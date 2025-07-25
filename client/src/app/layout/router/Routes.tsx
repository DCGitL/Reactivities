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
import LoginForm from "../../../features/account/LoginForm";
import RequireAuth from "./RequireAuth";
import RegisterForm from "../../../features/account/RegisterForm";
import ProfilePage from "../../../features/profiles/ProfilePage";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App/>,
        children: [
            {element: <RequireAuth/>, children: [
               {path: 'activities', element: <ActivityDashboard/>},        
               {path: 'activities/:id', element: <ActivityDetailPage/>},
               {path: 'createactivity', element: <ActivityForm key='create'/>},
               {path: 'manage/:id', element: <ActivityForm/>},
               {path: 'profiles/:id', element: <ProfilePage/>},
            ]},
            {path: '', element: <HomePage/>},
            {path: 'counter', element: <Counter/>},
            {path: 'errors', element: <TestErrors/>},
            {path: 'not-found', element: <NotFound/>},
            {path: 'server-error', element: <ServerError/>},
            {path: 'login', element: <LoginForm/>},
            {path: 'Register', element: <RegisterForm/>},
            {path: '*', element: <Navigate replace to='/not-found'/>},


        ]
    }
])