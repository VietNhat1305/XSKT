const toJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
        name:item.name,
        thuTu: item.thuTu,
        loaiAnhId: item.loaiAnhId,
    }
}
const fromJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
        name:item.name,
        thuTu: item.thuTu,
        loaiAnhId: item.loaiAnhId,
    }
}

const baseJson = () => {
    return {
        _id: null,
        file: [],
        name: null,
        thuTu: 0,
        loaiAnhId: null,
    }
}


export const  thuvienanhModel = {
    toJson, fromJson, baseJson
}
