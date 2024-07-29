import {apiClient} from "@/state/modules/apiClient";

const controller = "EditImg";
export const actions = {
    async create({commit}, values) {
        return apiClient.post(controller + "/create", values);
    },
    async update({commit}, values) {
        return apiClient.post(controller + "/update", values);
    },
    async getbydate({commit}, values) {
        return apiClient.post(controller + "/get-by-date", values);
    },
};
