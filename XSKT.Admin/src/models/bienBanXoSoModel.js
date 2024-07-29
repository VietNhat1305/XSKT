const toJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
        name:item.name,
    }
}
const fromJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
        name:item.name,
    }
}

const baseJson = () => {
    return {
        _id: null,
        file: null,
        name:null,
    }
}


export const  bienBanXoSoModel = {
    toJson, fromJson, baseJson
}
