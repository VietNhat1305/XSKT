const toJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
        name:item.name,
        moTa: item.moTa,
        thuTu: item.thuTu,
    }
}
const fromJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
        name:item.name,
        moTa: item.moTa,
        thuTu: item.thuTu,
    }
}

const baseJson = () => {
    return {
        _id: null,
        file: null,
        name:null,
        moTa: null,
        thuTu: 0,
    }
}


export const  hinhAnhModel = {
    toJson, fromJson, baseJson
}
