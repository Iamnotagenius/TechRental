import { IUser, IUserPage } from "../../shared/models";
import { CreateProfileDto } from "../../shared/dto";
import axios from "axios";

let url = process.env.API_URL

if (process.env.NODE_ENV === 'development')
	url += 'User'
else url += 'api/User'

export const api = axios.create({ baseURL: url })

export const getUser = async (id: string) => {
	return await api.get<IUser>(id)
}

export const getAllUsers = async (token: string, page?: number) => {
	return await api.get<IUserPage>(`${page}`, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}

export const createUserProfile = async (token: string, id: string, dto: CreateProfileDto) => {
	return await api.post<IUser>(`${id}/profile`, dto, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}

export const addProduct = async (token: string, id: string, productId: string) => {
	return await api.put(`${id}/orders`, productId, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}

export const removeProduct = async (token: string, id: string, productId: string) => {
	return await api.delete(`${id}/orders`, {
		headers: {
			Authorization: `Bearer ${token}`
		},
		data: {
			source: productId
		}
	})
}

export const replenishBalance = async (token: string, id: string, total: number) => {
	return await api.put(`${id}/account`, total, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}