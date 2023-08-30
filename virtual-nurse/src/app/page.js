/* eslint-disable react/jsx-no-undef */
import Cruz from '../../public/images/cruzRoja.jpg'
import Image from 'next/image'

export default function Home() {
  return (
    <div className="bg-[#DEF5E5] h-full w-full">
      <div className='lg:flex lg:flex-row-reverse'>
        <div className='p-5 text-black md:flex md:text-2xl md:pt-16 lg:flex lg:flex-wrap '>
          <h3 className='text-[20px] lg:w-[20vh]'>
            Bienvenidos a Virtual Nurse, nuestro objetivo es ayudarlo en cuanto
            a su salud procurando su vienestar, brindamos servicios a domicilio
            con personla calificado y de acuerdo al tipo de emergencia.
          </h3>
          <div className='h-[20vh] flex items-center justify-center'>
          <button className="bg-[#8EC3B0] hover:bg-green-200 text-black font-bold py-2 px-4 rounded-full transition duration-150 ease-in-out ">
            INFORMACION
          </button>
          <button className="ml-5 bg-[#8EC3B0] hover:bg-green-200 text-black font-bold py-2 px-4 rounded-full transition duration-150 ease-in-out ">
            COMENZAR
          </button>  
        </div>
        </div>
      
        <div className='flex justify-center items-center lg:w-[180vh] lg:h-[50vh] ml-15'>
          <Image className='bg-white rounded-full lg:w-[50vh] lg:h-[50vh]' src={Cruz} width={'19%'} height={'auto'} alt="" />
        </div>
      </div>
    </div>
  )
}
